<template>
  <main class="forum-layout">
    <ForumLayoutNavigation
      :sections="sections"
      class="forum-layout__navigation"
    />
    <router-view
      :class="{ 'loading': isLoading }"
      class="forum-layout__view"
      @update="onUpdate"
    />
    <ForumLayoutUserCard
      :is-additional-load-required="isAdditionalLoadRequired"
      :threads="userThreads"
      class="forum-layout__user-card"
      @input="onImageInput"
    />
  </main>
</template>

<script>
import ForumLayoutNavigation from './ForumLayoutNavigation'
import ForumLayoutUserCard from './ForumLayoutUserCard'
import api from '@/api'
import { imagesHelper } from '@/utils'

const defaultCount = 100
const delta = 300


export default {
  name: 'ForumLayout',
  components: {
    ForumLayoutNavigation,
    ForumLayoutUserCard
  },
  data () {
    return {
      sections: [],
      nextIndex: 0,
      userThreads: [],
      isThreadsLoading: false,
      isAdditionalLoadRequired: true
    }
  },
  computed: {
    isLoading () {
      return this.$store.getters.isLoading
    }
  },
  async created () {
    this.$store.commit('SET_MAIN_LOADING', true)
    await Promise.all([
      this.setSections(),
      this.loadThreads()
    ])
    this.$store.commit('SET_MAIN_LOADING', false)
    window.addEventListener('scroll', this.onScroll)
  },
  destroyed () {
    window.removeEventListener('scroll', this.onScroll)
  },
  methods: {
    async setSections () {
      try {
        const response = await api.sections.get.sections()
        this.sections = response.data.sections
      } catch {
        this.$router.replace({ name: 'Error500' })
      }
    },
    async onImageInput (data) {
      await imagesHelper.sendData(data, this.onImageCallback)
    },
    async onImageCallback (response) {
      const data = JSON.parse(response)

      try {
        await api.users.patch.image(data.externalId)
        window.location.reload()
      } catch {
        this.$router.replace({ name: 'Error500' })
      }
    },
    async onScroll () {
      const html = document.documentElement

      if (html.scrollTop + html.offsetHeight + delta >= html.scrollHeight && !this.isThreadsLoading && this.isAdditionalLoadRequired) {
        await this.loadTransactions()
      }
    },
    async loadThreads () {
      try {
        this.isThreadsLoading = true
        const response = await api.users.get.threads(this.nextIndex, defaultCount)
        const { threadsInfo } = response.data
        this.userThreads.push(...threadsInfo)
        this.isAdditionalLoadRequired &&= threadsInfo.length === defaultCount
        this.nextIndex += defaultCount
      } catch {
        this.isAdditionalLoadRequired = false
        // Igonre
      } finally {
        this.isThreadsLoading = false
      }
    },
    async onUpdate () {
      this.nextIndex = 0
      this.userThreads = []
      this.isThreadsLoading = false
      this.isAdditionalLoadRequired = true
      await this.loadThreads()
    }
  }
}
</script>

<style lang="scss">
.forum-layout {
  max-width: 1920px;
  height: 100%;
  display: grid;
  grid-template-columns: 1fr 3fr 1fr;

  &__navigation {
    border-right: 3px solid $fur-border;
  }

  &__user-card {
    border-left: 3px solid $fur-border;
  }
}
</style>
