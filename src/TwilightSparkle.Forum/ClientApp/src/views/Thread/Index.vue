<template>
  <article class="thread">
    <div class="thread__row">
      <h2> {{ title }} </h2>
      <div class="thread__aside">
        <AppButton
          v-if="isAuthor"
          color="secondary"
          @click="onDeleteThread"
        >
          Delete
        </AppButton>
        <img
          v-if="isAuthorized"
          :src="likeImage"
          class="thread__rate"
          @click="() => onRate(true)"
        >
        <span
          :class="likesClass"
          class="thread__likes"
        >
          {{ likesDislikes }}
        </span>
        <img
          v-if="isAuthorized"
          :src="dislikeImage"
          class="thread__rate"
          @click="() => onRate(false)"
        >
        <span> By: {{ authorUsername }} </span>
      </div>
    </div>
    <span class="thread__date">
      <span> {{ dateTime }} </span>
    </span>
    <section
      v-html="content"
      class="thread__content"
    />
    <h3> Comments: </h3>
    <ThreadComments
      :get-comments-api-method="getCommentsApiMethod"
      :thread-id="id"
      :key="commentsKey"
      @add-comment="onAddComment"
    />
  </article>
</template>

<script>
import ThreadComments from './ThreadComments'
import AppButton from '@/components/AppButton'
import api from '@/api'
import marked from 'marked'
import { datesHelper } from '@/utils'
import likeImage from '@/assets/thumb-up.png'
import dislikeImage from '@/assets/thumb-down.png'

export default {
  name: 'Thread',
  components: {
    ThreadComments,
    AppButton
  },
  props: {
    id: {
      type: [Number, String],
      required: true
    },
    sectionName: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      title: '',
      content: '',
      authorUsername: '',
      dateTime: '',
      likesDislikes: 0,
      getCommentsApiMethod: api.threads.get.comments,
      commentsKey: 0,
      likeImage,
      dislikeImage
    }
  },
  computed: {
    isAuthor () {
      return this.$store.getters.userData &&
        this.$store.getters.userData.username === this.authorUsername
    },
    isAuthorized () {
      return this.$store.getters.userData
    },
    likesClass () {
      if (this.likesDislikes > 0) {
        return 'thread__likes--positive'
      }

      if (this.likesDislikes < 0) {
        return 'thread__likes--negative'
      }

      return ''
    }
  },
  async created () {
    await this.setThread()
  },
  methods: {
    async setThread () {
      this.$store.commit('SET_LOADING', true)
      try {
        const response = await api.threads.get.byId(this.id)
        this.setData(response.data)
      } catch {
        this.$router.replace({ name: 'Error404' })
      } finally {
        this.$store.commit('SET_LOADING', false)
      }
    },
    setData (data) {
      const { title, content, authorUsername, creationDateTimeUtc, likesDislikes, sectionName } = data

      if (this.sectionName !== sectionName) {
        throw new Error()
      }

      this.title = title
      this.content = marked(content)
      this.authorUsername = authorUsername
      this.dateTime = datesHelper.getDateTimeFromDateString(creationDateTimeUtc)
      this.likesDislikes = likesDislikes
    },
    async onAddComment (content) {
      try {
        this.$store.commit('SET_LOADING', true)
        await api.threads.post.comment(this.id, { content })
      } catch {
        this.$router.replace({ name: 'Error500' })
      } finally {
        this.commentsKey += 1
        this.$store.commit('SET_LOADING', false)
      }
    },
    async onDeleteThread () {
      try {
        this.$store.commit('SET_LOADING', true)
        await api.threads.delete.thread(this.id)
        this.$router.replace({ name: 'Home' })
      } catch {
        this.$router.replace({ name: 'Error500' })
      } finally {
        this.$store.commit('SET_LOADING', false)
        this.$emit('update')
      }
    },
    async onRate (isPositive) {
      try {
        this.$store.commit('SET_LOADING', true)
        const response = await api.threads.post.rate(this.id, isPositive)
        this.setData(response.data)
      } catch {
        this.$router.replace({ name: 'Error500' })
      } finally {
        this.$store.commit('SET_LOADING', false)
      }
    }
  }
}
</script>

<style lang="scss">
.thread {
  padding: 20px 30px;

  &__row {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  &__aside {
    display: flex;
    align-items: center;
    padding: 20px;

    & > * + * {
      margin-left: 15px;
    }
  }

  &__rate {
    width: 30px;
    cursor: pointer;
  }

  &__likes {
    font-weight: 700;
    font-size: 20px;

    &--positive {
      color: green;
    }

    &--negative {
      color: red;
    }
  }
}
</style>
