<template>
  <div class="login-form">
      <label for="username">
        Username:
      </label>
      <AppInput
        name="username"
        v-model="model.username"
      />
      <label for="password">
        Password:
      </label>
      <AppInput
        name="password"
        v-model="model.password"
        type="password"
      />
      <div class="login-form__login-button">
        <AppButton
          :disabled="isButtonDisabled"
          @click="onLoginClick"
        >
          Login
        </AppButton>
      </div>
  </div>
</template>

<script>
import AppInput from '@/components/AppInput'
import AppButton from '@/components/AppButton'

export default {
  name: 'LoginForm',
  components: {
    AppInput,
    AppButton
  },
  data () {
    return {
      model: {
        username: '',
        password: ''
      }
    }
  },
  computed: {
    isButtonDisabled () {
      return this.model.username.length < 5 || this.model.password.length < 8
    }
  },
  methods: {
    async onLoginClick () {
      this.$emit('login', this.model)
    },
  },
}
</script>

<style lang="scss">
.login-form {
  display: flex;
  flex-direction: column;
  width: 480px;

  & > label {
    font-weight: $font-weight-medium;
  }

  > * + * {
    margin-top: 10px;
  }
}
</style>
