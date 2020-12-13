<template>
  <section class="register">
    <RegisterForm @register="onRegister" />
  </section>
</template>

<script>
import RegisterForm from '@/components/forms/RegisterForm'
import api from '@/api'

export default {
  name: 'Register',
  components: {
    RegisterForm
  },
  methods: {
    async onRegister (model) {
      this.$store.commit('SET_LOADING', true)
      await api.authentication.post.signUp(model)

      const response = await api.authentication.post.token({
        username: model.userName,
        password: model.password
      })

      const { access_token, refresh_token, expires_in } = response.data
      this.$cookies.set('token', access_token, expires_in)
      this.$cookies.remove('refresh-token')
      this.$cookies.set('refresh-token', refresh_token, 60 * 60 * 24 * 30)
      await this.$store.dispatch('SET_USER_DATA')
      this.$store.commit('SET_LOADING', false)
      this.$router.replace({ name: 'Home' })
    }
  }
}
</script>

<style lang="scss">
.register {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 30px;
}
</style>
