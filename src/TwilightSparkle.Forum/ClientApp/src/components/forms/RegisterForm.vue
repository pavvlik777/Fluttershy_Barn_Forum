<template>
  <div class="register-form">
    <div class="register-form__row">
      <label for="userName">
        UserName:
      </label>
      <AppInput
        v-model="model.userName"
        :invalid="userNameErrorText.length > 0"
        name="userName" 
        @input="$v.model.userName.$touch()"
      />
    </div>
    <span
      v-if="userNameErrorText.length"
      class="register-form__error-text"
    >
      {{ userNameErrorText }}
    </span>
    <div class="register-form__row">
      <label for="email">
        Email:
      </label>
      <AppInput
        v-model="model.email"
        :invalid="emailErrorText.length > 0"
        name="email"
        type="email"
        @input="$v.model.email.$touch()"
      />
    </div>
    <span
      v-if="emailErrorText.length"
      class="register-form__error-text"
    >
      {{ emailErrorText }}
    </span>
    <div class="register-form__row">
      <label for="password">
        Password:
      </label>
      <AppInput
        v-model="model.password"
        :invalid="passwordErrorText.length > 0"
        name="password"
        type="password"
        @input="$v.model.password.$touch()"
      />
    </div>
    <span
      v-if="passwordErrorText.length"
      class="register-form__error-text"
    >
      {{ passwordErrorText }}
    </span>
    <div class="register-form__row">
      <label for="passwordConfirmation">
        Repeat password:
      </label>
      <AppInput
        v-model="model.passwordConfirmation"
        :invalid="passwordConfirmationErrorText.length > 0"
        name="passwordConfirmation"
        type="password"
        @input="$v.model.passwordConfirmation.$touch()"
      />
    </div>
    <span
      v-if="passwordConfirmationErrorText.length"
      class="register-form__error-text"
    >
      {{ passwordConfirmationErrorText }}
    </span>
    <div class="register-form__row register-form__row--button">
      <AppButton
        :disabled="$v.model.$invalid"
        @click="onRegisterClick"
      >
        Register
      </AppButton>
    </div>
  </div>
</template>

<script>
import AppInput from '@/components/AppInput'
import AppButton from '@/components/AppButton'
import { required, minLength, maxLength, email, sameAs } from 'vuelidate/lib/validators'

const allowedSymbols = /^[a-zA-Z0-9._]+$/
const uppercaseSymbols = /[A-Z]/
const lowercaseSymbols = /[a-z]/
const numberSymbols = /[0-9]/
const userNameMinLength = 5
const userNameMaxLength = 20
const passwordMinLength = 8

const matchExp = (value, regExp) => {
  return value.match(regExp)
    ? true
    : false
}

export default {
  name: 'RegisterForm',
  components: {
    AppInput,
    AppButton
  },
  data () {
    return {
      model: {
        userName: '',
        password: '',
        passwordConfirmation: '',
        email: ''
      }
    }
  },
  computed: {
    userNameErrorText () {
      const userName = this.$v.model.userName
      if (userName.$dirty && userName.$error) {
        if (!userName.required) {
          return 'Field is required.'
        }

        if (!userName.minLength) {
          return `Field should be at least ${userNameMinLength} symbols long.`
        }

        if (!userName.maxLength) {
          return `Field should be ${userNameMaxLength} symbols maximum.`
        }

        if (!userName.characters) {
          return "Field should contain 'a-z', 'A-Z', '0-9', '_', '.' symbols only."
        }
      }

      return ''
    },
    emailErrorText () {
      const email = this.$v.model.email

      if (email.$dirty && email.$error) {
        if (!email.required) {
          return 'Field is required.'
        }
        
        if (!email.email) {
          return 'Field should be an email.'
        }
      }

      return ''
    },
    passwordErrorText () {
      const password = this.$v.model.password

      if (password.$dirty && password.$error) {
        if (!password.required) {
          return 'Field is required.'
        }

        if (!password.minLength) {
          return `Field should be at least ${passwordMinLength} symbols long.`
        }

        if (!password.containsUppercase) {
          return 'Field should contain at least one uppercase symbol(en).'
        }

        if (!password.containsLowercase) {
          return 'Field should contain at least one lowercase symbol(en).'
        }

        if (!password.containsNumber) {
          return 'Field should contain at least one digit.'
        }
      }

      return ''
    },
    passwordConfirmationErrorText () {
      const password = this.$v.model.passwordConfirmation

      if (password.$dirty && password.$error) {
        if (!password.required) {
          return 'Field is required.'
        }

        if (!password.sameAsPassword) {
          return 'Field should be the same as Password.'
        }
      }

      return ''
    }
  },
  methods: {
    async onRegisterClick () {
      this.$emit('register', this.model)
    }
  },
  validations: {
    model: {
      userName: {
        required,
        minLength: minLength(userNameMinLength),
        maxLength: maxLength(userNameMaxLength),
        characters (value) {
          return matchExp(value, allowedSymbols)
        }
      },
      email: {
        required,
        email
      },
      password: {
        required,
        minLength: minLength(passwordMinLength),
        containsUppercase (value) {
          return matchExp(value, uppercaseSymbols)
        },
        containsLowercase (value) {
          return matchExp(value, lowercaseSymbols)
        },
        containsNumber (value) {
          return matchExp(value, numberSymbols)
        }
      },
      passwordConfirmation: {
        required,
        sameAsPassword: sameAs('password')
      }
    },
    validationGroup: ['model.userName', 'model.email', 'model.password', 'model.passwordConfirmation']
  }
}
</script>

<style lang="scss">
.register-form {
  display: flex;
  flex-direction: column;
  width: 480px;

  &__row {
    display: grid;
    grid-template-columns: 1fr 2fr;
    gap: 15px;
    margin-bottom: 10px;
    align-items: center;

    & > label {
      font-weight: 500;
    }

    & > .app-input {
      margin-left: 20px;
      width: auto;
    }

    &--button {
      justify-content: center;
    }
  }

  &__error-text {
    color: red;
    margin-bottom: 10px;
  }

}
</style>