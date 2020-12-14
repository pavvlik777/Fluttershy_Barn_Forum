<template>
  <router-link
    :to="to"
    class="app-link"
  >
    <slot />
  </router-link>
</template>

<script>
export default {
  name: 'AppLink',
  props: {
    to: {
      type: Object,
      required: true
    }
  }
}
</script>

<style lang="scss">
.app-link {
  padding: 5px;
  border: 3px solid $mane-border;
  border-radius: 4px;
  background-color: $fur-color;
  color: $black;
  text-decoration: none;

  &:hover {
    background-color: darken($fur-color, 10%);
  }
}
</style>
