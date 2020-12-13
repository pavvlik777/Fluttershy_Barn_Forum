<template>
  <ul class="thread-comments">
    <li
      v-for="comment in comments"
      :key="comment.commentDateUtc"
      class="thread-comments__list-item thread-comments__list-item--container"
    >
      <div class="thread-comments__row">
        <h4> {{ comment.authorNickname }} </h4>
        <span> {{ getDateTime(comment.commentTimeUtc) }} </span>
      </div>
      <span class="thread-comments__content">
        {{ comment.content }}
      </span>
    </li>
    <li
      v-if="isAuthorized"
      class="thread-comments__list-item"
    >
      <textarea
        v-model="userComment"
        class="thread-comments__textarea"
      />
      <AppButton @click="onCommentAdd">
        Add comment
      </AppButton>
    </li>
  </ul>
</template>

<script>
import AppButton from '@/components/AppButton'
import { datesHelper } from '@/utils'

const defaultCount = 100
const delta = 300

export default {
  name: 'ThreadComments',
  components: {
    AppButton
  },
  props: {
    getCommentsApiMethod: {
      type: Function,
      required: true
    },
    threadId: {
      type: [Number, String],
      required: true
    }
  },
  data () {
    return {
      comments: [],
      nextIndex: 0,
      isAdditionalLoadRequired: true,
      isLoading: false,
      userComment: ''
    }
  },
  computed: {
    isAuthorized () {
      return this.$store.getters.userData
    }
  },
  async created () {
    this.$store.commit('SET_LOADING', true)
    await this.loadComments()
    this.$store.commit('SET_LOADING', false)
    window.addEventListener('scroll', this.onScroll)
  },
  destroyed () {
    window.removeEventListener('scroll', this.onScroll)
  },
  methods: {
    async onScroll () {
      const html = document.documentElement

      if (html.scrollTop + html.offsetHeight + delta >= html.scrollHeight && !this.isLoading && this.isAdditionalLoadRequired) {
        await this.loadTransactions()
      }
    },
    async loadComments () {
      try {
        this.isLoading = true
        const response = await this.getCommentsApiMethod(this.nextIndex, defaultCount, this.threadId)
        const { comments } = response.data
        this.comments.push(...comments)
        this.isAdditionalLoadRequired &= comments.length === defaultCount
        this.nextIndex += defaultCount
      } catch {
        this.$router.replace({ name: 'Error404' })
      } finally {
        this.isLoading = false
      }
    },
    onCommentAdd () {
      this.$emit('add-comment', this.userComment)
    },
    getDateTime (dateString) {
      return datesHelper.getDateTimeFromDateString(dateString)
    }
  }
}
</script>

<style lang="scss">
.thread-comments {
  list-style-type: none;
  padding: 0;

  &__list-item {
    margin: 10px 0;
    display: flex;
    flex-direction: column;

    & > * + * {
      margin-top: 10px;
    }

    &--container {
      border: 3px solid $fur-border;
      padding: 5px 10px;
      border-radius: 6px;

      h4 {
        margin: 0;
      }
    }
  }

  &__row {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  &__textarea {
    resize: vertical;
    min-height: 60px;
    padding: 10px;
    border-radius: 4px;
  }
}
</style>
