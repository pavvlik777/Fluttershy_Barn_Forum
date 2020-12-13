<template>
  <button
    class="app-button"
    :class="buttonClass"
    @click="onButtonClick"
  >
    <slot />
  </button>
</template>

<script>
export default {
  name: 'AppButton',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    color: {
      type: String,
      default: 'primary'
    }
  },
  computed: {
    buttonClass () {
      const classes = [`app-button--${this.color}`]

      if (this.disabled) {
        classes.push('app-button--disabled')
      }

      return classes.join` `
    }
  },
  methods: {
    onButtonClick () {
      if (!this.disabled) {
        this.$emit('click')
      }
    }
  }
}
</script>

<style lang="scss">
.app-button {
  width: fit-content;
  padding: 5px 10px;
  border: 3px solid $mane-border;
  border-radius: 4px;
  background-color: $fur-color;
  cursor: pointer;

  &--disabled {
    cursor: default;
    background-color: $cutiemark-color !important;
    border: 1px solid $black;
  }

  &:hover {
    background-color: darken($fur-color, 10%);
  }

  &--secondary {
    background-color: $mane-color;
    border: 3px solid $fur-border;

    &:hover {
      background-color: darken($mane-color, 10%);
    }
  }
}
</style>