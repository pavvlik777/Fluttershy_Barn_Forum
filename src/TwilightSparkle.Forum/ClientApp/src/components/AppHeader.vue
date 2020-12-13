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
    <nav
      v-else
      class="app-header__navigation"
    >
    <AppLink :to="{ name: 'Register' }">
      Register
    </AppLink>
    <AppLink :to="{ name: 'Login' }">
      Login
    </AppLink>
    </nav>
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
      return this.$store.getters.userData
    }
  },
  methods: {
    onLogout () {
      this.$cookies.remove('token')
      this.$cookies.remove('refresh-token')
      this.$store.commit('SET_USER_DATA', { userData: null })
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

  &__navigation {
    display: flex;
    & > * + * {
      margin-left: 15px;
    }
  }

  &__text {
    color: $black;
    padding: 5px;
  }
}
</style>
