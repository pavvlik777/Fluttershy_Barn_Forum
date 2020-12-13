<template>
  <footer class="app-footer">
    <span class="app-footer__text">
      Pavel Astapenka, Artyom Popyol © 2020
    </span>
  </footer>
</template>

<script>
export default {
  name: 'AppFooter'
}
</script>

<style lang="scss">
.app-footer {
  display: flex;
  background-color: $mane-color;
  justify-content: center;
  z-index: 3;
  border-top: 3px solid $mane-border;

  &__text {
    color: $black;
    padding: 5px;
  }
}
</style>