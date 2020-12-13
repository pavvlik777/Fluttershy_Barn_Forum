<template>
  <div>
    <AppHeader />
    <main :class="{ 'loading': isUserDataLoading }" >
      <router-view class="main-layout__main" />
    </main>
    <AppFooter class="main-layout__footer" />
  </div>
</template>

<script>
import AppHeader from '@/components/AppHeader'
import AppFooter from '@/components/AppFooter'
import api from '@/api'

export default {
  name: 'MainLayout',
  components: {
    AppHeader,
    AppFooter
  },
  created () {
    api.heartbeat.get.date()
  }
}
</script>

<style lang="scss">
.main-layout {
  &__footer {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
  }

  &__main {
    padding-bottom: 50px !important;
  }
}
</style>