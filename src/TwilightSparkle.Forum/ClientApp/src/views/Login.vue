<template>
  <section
    :class="loginClass"
    class="login"
  >
    <LoginForm @login="onLogin" />
    <p
      v-if="isError"
      class="login__error"
    >
      Invalid credentials.
    </p>
  </section>
</template>

<script>
import LoginForm from '@/components/forms/LoginForm'
import api from '@/api'

export default {
  name: 'Login',
  components: {
    LoginForm
  },
  data () {
    return {
      isLoading: false,
      isError: false
    }
  },
  computed: {
    loginClass () {
      return this.isLoading
        ? 'loading'
        : ''
    }
  },
  methods: {
    async onLogin (model) {
      this.isLoading = true
      this.isError = false
      try {
        const response = await api.authentication.post.token(model)

        const { access_token, refresh_token, expires_in } = response.data
        this.$cookies.set('token', access_token, expires_in)
        this.$cookies.remove('refresh-token')
        this.$cookies.set('refresh-token', refresh_token, 60 * 60 * 24 * 30)
        await this.$store.dispatch('SET_USER_DATA')
        this.$router.replace({ name: 'Home' })
      } catch (e) {
        this.isError = true
      } finally {
        this.isLoading = false
      }
    }
  }
}
</script>

<style lang="scss">
.login {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 30px;

  &__error {
    color: red;
  }
}
</style>
