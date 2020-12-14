using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.ThreadsService
{
    public class ThreadsManagementService : IThreadsManagementService
    {
        private readonly IForumUnitOfWork _unitOfWork;

        private const int MaxGetQuerySize = 100;


        public ThreadsManagementService(IForumUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResult<ThreadsInfo, GetThreadsInfoError>> GetPopularThreads(int startIndex, int size)
        {
            if (startIndex < 0 || size > MaxGetQuerySize)
            {
                return ServiceResult<ThreadsInfo, GetThreadsInfoError>.CreateFailed(GetThreadsInfoError.InvalidPaginationArguments);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var threads = await threadRepository.All(null, t => t.Likes, t => t.Author, t => t.Section)
                .OrderByDescending(t => t.Likes.Where(l => l.IsLike).Count() - t.Likes.Where(l => !l.IsLike).Count())
                .Skip(startIndex).Take(size).ToListAsync();
            var result = new ThreadsInfo
            {
                Amount = threads.Count,
                Threads = threads.Select(t => new ThreadInfo
                {
                    Id = t.Id,
                    Title = t.Title,
                    Content = t.Content,
                    SectionName = t.Section.Name,
                    CreationDateTimeUtc = t.CreationDateTimeUtc,
                    AuthorUsername = t.Author.Username,
                    LikesDislikes = t.Likes.Where(l => l.IsLike).Count() - t.Likes.Where(l => !l.IsLike).Count()
                }).ToList(),
                Size = size,
                StartIndex = startIndex
            };

            return ServiceResult<ThreadsInfo, GetThreadsInfoError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<ThreadsCount, GetThreadsCountError>> GetPopularThreadsCount()
        {
            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var threads = threadRepository.All(null);
            var count = await threads.CountAsync();
            var result = new ThreadsCount
            {
                Count = count
            };

            return ServiceResult<ThreadsCount, GetThreadsCountError>.CreateSuccess(result);
        }


        public async Task<ServiceResult<ThreadInfo, GetThreadInfoError>> GetThreadInfo(int threadId)
        {
            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId, t => t.Author, t => t.Likes, t => t.Section);
            if (thread == null)
            {
                return ServiceResult<ThreadInfo, GetThreadInfoError>.CreateFailed(GetThreadInfoError.NotFound);
            }

            var result = new ThreadInfo
            {
                Id = thread.Id,
                Title = thread.Title,
                Content = thread.Content,
                SectionName = thread.Section.Name,
                CreationDateTimeUtc = thread.CreationDateTimeUtc,
                AuthorUsername = thread.Author.Username,
                LikesDislikes = thread.Likes.Where(l => l.IsLike).Count() - thread.Likes.Where(l => !l.IsLike).Count()
            };

            return ServiceResult<ThreadInfo, GetThreadInfoError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<ThreadInfo, CreateThreadError>> CreateThread(CreateThreadDto request)
        {
            var sectionRepository = _unitOfWork.GetRepository<Section>();
            var section = await sectionRepository.FirstOrDefaultAsync(s => s.Name == request.SectionName);
            if (section == null)
            {
                return ServiceResult<ThreadInfo, CreateThreadError>.CreateFailed(CreateThreadError.SectionNotFound);
            }

            var userRepository = _unitOfWork.GetRepository<User>();
            var author = await userRepository.FirstOrDefaultAsync(s => s.Username == request.AuthorUsername);
            if (author == null)
            {
                return ServiceResult<ThreadInfo, CreateThreadError>.CreateFailed(CreateThreadError.UserNotFound);
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return ServiceResult<ThreadInfo, CreateThreadError>.CreateFailed(CreateThreadError.InvalidTitle);
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return ServiceResult<ThreadInfo, CreateThreadError>.CreateFailed(CreateThreadError.InvalidContent);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var creationDateTimeUtc = DateTime.UtcNow;
            var thread = new Thread
            {
                Title = request.Title,
                Content = request.Content,
                Author = author,
                Section = section,
                CreationDateTimeUtc = creationDateTimeUtc
            };

            threadRepository.Create(thread);

            await _unitOfWork.SaveAsync();

            var threadInfo = new ThreadInfo
            {
                Id = thread.Id,
                Title = thread.Title,
                Content = thread.Content,
                SectionName = section.Name,
                CreationDateTimeUtc = creationDateTimeUtc,
                AuthorUsername = author.Username,
                LikesDislikes = 0
            };

            return ServiceResult<ThreadInfo, CreateThreadError>.CreateSuccess(threadInfo);
        }

        public async Task<ServiceResult<DeleteThreadError>> DeleteThread(int threadId, string username)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.FirstOrDefaultAsync(s => s.Username == username);
            if (user == null)
            {
                return ServiceResult<DeleteThreadError>.CreateFailed(DeleteThreadError.UserNotFound);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId, t => t.Author);
            if (thread == null)
            {
                return ServiceResult<DeleteThreadError>.CreateFailed(DeleteThreadError.ThreadNotFound);
            }

            if (thread.AuthorId != user.Id)
            {
                return ServiceResult<DeleteThreadError>.CreateFailed(DeleteThreadError.UserNotAuthor);
            }

            threadRepository.Delete(thread);
            await _unitOfWork.SaveAsync();

            return ServiceResult<DeleteThreadError>.CreateSuccess();
        }


        public async Task<ServiceResult<ThreadInfo, LikeDislikeThreadError>> LikeDislikeThread(int threadId, string username, bool isLike)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.FirstOrDefaultAsync(s => s.Username == username);
            if (user == null)
            {
                return ServiceResult<ThreadInfo, LikeDislikeThreadError>.CreateFailed(LikeDislikeThreadError.UserNotFound);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId, t => t.Author);
            if (thread == null)
            {
                return ServiceResult<ThreadInfo, LikeDislikeThreadError>.CreateFailed(LikeDislikeThreadError.ThreadNotFound);
            }

            var likesRepository = _unitOfWork.GetRepository<LikeDislike>();
            var currentLike = await likesRepository.FirstOrDefaultAsync(l => l.ThreadId == threadId && l.User == user, l => l.Thread, l => l.User);
            if (currentLike == null)
            {
                currentLike = new LikeDislike
                {
                    User = user,
                    Thread = thread,
                    IsLike = isLike
                };

                likesRepository.Create(currentLike);
            }
            else if (currentLike.IsLike != isLike)
            {
                currentLike.IsLike = isLike;

                likesRepository.Update(currentLike);
            }
            else
            {
                likesRepository.Delete(currentLike);
            }

            await _unitOfWork.SaveAsync();

            var updatedThread = await threadRepository.FirstOrDefaultAsync(t => t.Id == thread.Id, t => t.Author, t => t.Likes, t => t.Section);
            var result = new ThreadInfo
            {
                Id = updatedThread.Id,
                Title = updatedThread.Title,
                Content = updatedThread.Content,
                SectionName = updatedThread.Section.Name,
                CreationDateTimeUtc = updatedThread.CreationDateTimeUtc,
                AuthorUsername = updatedThread.Author.Username,
                LikesDislikes = updatedThread.Likes.Where(l => l.IsLike).Count() - thread.Likes.Where(l => !l.IsLike).Count()
            };

            return ServiceResult<ThreadInfo, LikeDislikeThreadError>.CreateSuccess(result);
        }


        public async Task<ServiceResult<ThreadCommentsInfo, GetThreadCommentsError>> GetThreadCommentsInfo(int threadId, int startIndex, int size)
        {
            if (startIndex < 0 || size > MaxGetQuerySize)
            {
                return ServiceResult<ThreadCommentsInfo, GetThreadCommentsError>.CreateFailed(GetThreadCommentsError.InvalidPaginationArguments);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId);
            if (thread == null)
            {
                return ServiceResult<ThreadCommentsInfo, GetThreadCommentsError>.CreateFailed(GetThreadCommentsError.ThreadNotFound);
            }

            var commentRepository = _unitOfWork.GetRepository<Commentary>();
            var comments = await commentRepository.All(c => c.ThreadId == threadId, c => c.Author)
                .OrderByDescending(c => c.CommentTimeUtc).Skip(startIndex).Take(size).ToListAsync();
            var result = new ThreadCommentsInfo
            {
                Amount = comments.Count,
                Comments = comments.Select(c => new ThreadCommentInfo
                {
                    AuthorNickname = c.Author.Username,
                    Content = c.Content,
                    CommentTimeUtc = c.CommentTimeUtc
                }).ToList(),
                Size = size,
                StartIndex = startIndex
            };

            return ServiceResult<ThreadCommentsInfo, GetThreadCommentsError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<ThreadCommentsCount, GetThreadCommentsCountError>> GetThreadCommentsCount(int threadId)
        {
            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId, t => t.Commentaries);
            if (thread == null)
            {
                return ServiceResult<ThreadCommentsCount, GetThreadCommentsCountError>.CreateFailed(GetThreadCommentsCountError.ThreadNotFound);
            }

            var commentRepository = _unitOfWork.GetRepository<Commentary>();
            var comments = commentRepository.All(c => c.ThreadId == threadId, c => c.Author);
            var count = await comments.CountAsync();
            var result = new ThreadCommentsCount
            {
                Count = count
            };

            return ServiceResult<ThreadCommentsCount, GetThreadCommentsCountError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<ThreadCommentInfo, CommentThreadError>> CommentThread(CommentThreadDto request)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.FirstOrDefaultAsync(s => s.Username == request.AuthorUsername);
            if (user == null)
            {
                return ServiceResult<ThreadCommentInfo, CommentThreadError>.CreateFailed(CommentThreadError.UserNotFound);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == request.ThreadId, t => t.Commentaries);
            if (thread == null)
            {
                return ServiceResult<ThreadCommentInfo, CommentThreadError>.CreateFailed(CommentThreadError.ThreadNotFound);
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return ServiceResult<ThreadCommentInfo, CommentThreadError>.CreateFailed(CommentThreadError.InvalidContent);
            }

            var commentRepository = _unitOfWork.GetRepository<Commentary>();
            var commentTimeUtc = DateTime.UtcNow;
            var newComment = new Commentary
            {
                Author = user,
                Thread = thread,
                CommentTimeUtc = commentTimeUtc,
                Content = request.Content
            };
            commentRepository.Create(newComment);

            await _unitOfWork.SaveAsync();
            var result = new ThreadCommentInfo
            {
                Content = newComment.Content,
                AuthorNickname = user.Username,
                CommentTimeUtc = commentTimeUtc
            };

            return ServiceResult<ThreadCommentInfo, CommentThreadError>.CreateSuccess(result);
        }
    }
}
