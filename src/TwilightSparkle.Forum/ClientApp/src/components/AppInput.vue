<template>
  <input
    v-bind="options"
    :type="type"
    :value="value"
    :class="appInputClass"
    class="app-input"
    @input="onInput"
  >
</template>

<script>
export default {
  name: 'AppInput',
  props: {
    type: {
      type: String,
      default: 'text'
    },
    value: {
      type: [String, Number],
      default: ''
    },
    invalid: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {}
    }
  },
  computed: {
    appInputClass () {
      const classes = []

      if (this.invalid) {
        classes.push('app-input--invalid')
      }

      return classes.join` `
    }
  },
  methods: {
    onInput (event) {
      this.$emit('input', event.target.value)
    }
  }
}
</script>

<style lang="scss">
.app-input {
  padding: 5px;
  border-radius: 4px;
  border: 1px solid $fur-border;
  width: 100%;

  &--invalid {
    border: 2px solid red !important;
  }

  &:focus {
    border: 2px solid $eye-color;
    outline: none;
  }
}
</style>