<template>
  <header class="app-header">
    <AppLink :to="{ name: 'Home' }">
      Home
    </AppLink>
    <AppButton
      v-if="isLoggedIn"
      @click="onLogout"
    >
      Log Out
    </AppButton>
    <navigation
      v-else
      class="app-header__navigation"
    >
    <AppLink :to="{ name: 'Register' }">
      Register
    </AppLink>
    <AppLink :to="{ name: 'Login' }">
      Login
    </AppLink>
    </navigation>
  </header>
</template>

<script>
import AppButton from '@/components/AppButton'
import AppLink from '@/components/AppLink'

export default {
  name: 'AppHeader',
  components: {
    AppButton,
    AppLink
  },
  computed: {
    isLoggedIn () {
      return true
    }
  },
  methods: {
    onLogout () {
      console.log('on-logout')
    }
  }
}
</script>

<style lang="scss">
.app-header {
  display: flex;
  background-color: $mane-color;
  justify-content: space-between;
  z-index: 3;
  padding: 5px 10px;
  border-bottom: 3px solid $mane-border;
  
  .app-link,
  .app-button {
    font-size: 15px;
    font-weight: 500;
  }

  &__text {
    color: $black;
    padding: 5px;
  }
}
</style>
