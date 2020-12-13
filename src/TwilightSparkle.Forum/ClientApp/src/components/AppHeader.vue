<template>
  <header class="app-header">
    <span class="app-footer__text">
      Header
    </span>
  </header>
</template>

<script>
export default {
  name: 'AppHeader'
}
</script>

<style lang="scss">
.app-header {
  display: flex;
  background-color: $mane-color;
  justify-content: center;
  z-index: 3;

  &__text {
    color: $black;
    padding: 5px;
  }
}
</style>
