<template>
  <article class="section">
    <div
      :class="headerClass"
      class="section__header"
    >
      <h2> {{ name }} </h2>
      <span
        v-if="isAuthorized"
        class="section__add-button"
        @click="onAddThread"
      >
        +
      </span>
    </div>
    <ul class="section__list">
      <li
        v-for="thread in threads"
        :key="thread.id"
        class="section__list-item"
      >
        <SectionThread
          v-bind="thread"
          @click="onOpenThread"
        />
      </li>
      <li
        v-if="isAdditionalLoadRequired"
        class="section__item-loading loading"
      />
    </ul>
  </article>
</template>

<script>
import SectionThread from './SectionThread'
import api from '@/api'

const defaultCount = 100
const delta = 300

export default {
  name: 'Section',
  components: {
    SectionThread
  },
  props: {
    name: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      threads: [],
      nextIndex: 0,
      isAdditionalLoadRequired: true,
      isLoading: false
    }
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
  computed: {
    isAuthorized () {
      return this.$store.getters.userData
    },
    headerClass () {
      return this.isAuthorized
        ? 'section__header--space'
        : ''
    }
  },
  watch: {
    async name () {
      this.threads = []
      this.nextIndex = 0,
      this.isAdditionalLoadRequired = true
      this.isLoading = false

      this.$store.commit('SET_LOADING', true)
      await this.loadThreads()
      this.$store.commit('SET_LOADING', false)
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
        const response = await api.sections.get.byName(this.nextIndex, defaultCount, this.name)
        const { threads } = response.data
        this.threads.push(...threads)
        this.isAdditionalLoadRequired &= threads.length === defaultCount
        this.nextIndex += defaultCount
      } catch {
        this.$router.replace({ name: 'Error404' })
      } finally {
        this.isLoading = false
      }
    },
    onAddThread () {
      this.$router.push({ name: 'CreateThread', params: { sectionName: this.name }})
    },
    onOpenThread (id) {
      this.$router.push({ name: 'Thread', params: { sectionName: this.name, id }})
    }
  }
}
</script>

<style lang="scss">
.section {
  display: flex;
  flex-direction: column;
  padding: 20px 30px;

  &__add-button {
    height: 30px;
    width: 30px;
    text-align: center;
    font-size: 30px;
    font-weight: 700;
    padding: 10px;
    border-radius: 100px;
    border: 1px solid black;
    cursor: pointer;
    display: flex;
    justify-content: center;
    align-items: center;
  }

  &__header {
    display: flex;
    align-items: center;

    &--space {
      justify-content: space-between;
    }
  }

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
