<template>
  <section class="error-404">
    <img
      class="error-404__image"
      :src="image"
    >
    <p class="error-404__text">
      This place does not exist in Equestria.<br/>Let's trot back home, pony.
    </p>
    <AppButton @click="onButtonClick">
      Go Home
    </AppButton>
  </section>
</template>

<script>
import image from '@/assets/404.png'
import AppButton from '@/components/AppButton'

export default {
  name: 'Error404',
  components: {
    AppButton
  },
  data () {
    return {
      image
    }
  },
  methods: {
    onButtonClick () {
      this.$router.push({ name: 'Home' })
    }
  }
}
</script>

<style lang="scss">
.error-404 {
  display: flex;
  flex-direction: column;
  align-items: center;

  &__text {
    font-weight: $font-weight-medium;
  }
}
</style>
