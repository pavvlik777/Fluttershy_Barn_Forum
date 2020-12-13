<template>
  <main class="forum-layout">
    <ForumLayoutNavigation
      :sections="sections"
      class="forum-layout__navigation"
    />
    <router-view
      :class="{ 'loading': isLoading }"
      class="forum-layout__view"
    />
    <ForumLayoutUserCard
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

export default {
  name: 'ForumLayout',
  components: {
    ForumLayoutNavigation,
    ForumLayoutUserCard
  },
  data () {
    return {
      sections: []
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
      this.setSections()
    ])
    this.$store.commit('SET_MAIN_LOADING', false)
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
