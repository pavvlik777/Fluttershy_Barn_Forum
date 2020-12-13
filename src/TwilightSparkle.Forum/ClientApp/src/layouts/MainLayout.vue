<template>
  <div class="main-layout">
    <AppHeader />
    <main
      :class="{ 'loading': isUserDataLoading }"
      class="main-layout__main"
    >
      <router-view />
    </main>
    <AppFooter class="main-layout__footer" />
  </div>
</template>

<script>
import AppHeader from '@/components/AppHeader'
import AppFooter from '@/components/AppFooter'
import { datesHelper } from '@/utils'

export default {
  name: 'MainLayout',
  components: {
    AppHeader,
    AppFooter
  },
  created () {
    console.log(datesHelper.getDateTimeFromDateString(new Date()))
  },
  computed: {
    isUserDataLoading () {
      return this.$store.getters.isUserDataLoading
    }
  }
}
</script>

<style lang="scss">
.main-layout {
  height: 100%;

  &__footer {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
  }

  &__main {
    height: calc(100% - 46px - 28px);
  }
}
</style>