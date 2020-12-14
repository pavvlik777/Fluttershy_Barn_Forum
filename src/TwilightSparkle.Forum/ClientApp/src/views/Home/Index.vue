<template>
  <article class="home">
    <h2> Popular Threads </h2>
    <ul class="home__list">
      <li
        v-for="thread in threads"
        :key="thread.id"
        class="home__list-item"
      >
        <ThreadItem
          class="home__thread"
          v-bind="thread"
          @click="() => onThreadClick(thread)"
        />
      </li>
      <li
        v-if="isAdditionalLoadRequired"
        class="home__item-loading loading"
      />
    </ul>
  </article>
</template>

<script>
import ThreadItem from '@/components/ThreadItem'
import api from '@/api'

const defaultCount = 100
const delta = 300

export default {
  name: 'Home',
  components: {
    ThreadItem
  },
  async created () {
    this.$store.commit('SET_LOADING', true)
    await this.loadThreads()
    this.$store.commit('SET_LOADING', false)
    window.addEventListener('scroll', this.onScroll)
  },
  destroyed () {
    window.removeEventListener('scroll', this.onScroll)
  },
  data () {
    return {
      threads: [],
      nextIndex: 0,
      isAdditionalLoadRequired: true,
      isLoading: false
    }
  },
  methods: {
    async onScroll () {
      const html = document.documentElement

      if (html.scrollTop + html.offsetHeight + delta >= html.scrollHeight && !this.isLoading && this.isAdditionalLoadRequired) {
        await this.loadTransactions()
      }
    },
    async loadThreads () {
      try {
        this.isLoading = true
        const response = await api.threads.get.popular(this.nextIndex, defaultCount, this.name)
        const { threads } = response.data
        this.threads.push(...threads)
        this.isAdditionalLoadRequired &= threads.length === defaultCount
        this.nextIndex += defaultCount
      } catch {
        this.$router.replace({ name: 'Error500' })
      } finally {
        this.isLoading = false
      }
    },
    onThreadClick (thread) {
      this.$router.push({
        name: 'Thread',
        params: {
          sectionName: thread.sectionName,
          id: thread.id
        }
      })
    }
  }
}
</script>

<style lang="scss">
.home {
  padding: 20px 30px;
  display: flex;
  flex-direction: column;

  &__item-loading.loading {
    display: block;
    width: 300px;
    height: 335px;
    position: relative;

    &::before {
      transform: scale(0.5);
    }
  }

  &__list {
    list-style-type: none;
    padding: 0;
  }

  &__list-item {
    margin: 5px 0;
  }
}
</style>
