<template>
  <section class="error-500">
    <img
      class="error-500__image"
      :src="image"
    >
    <AppButton @click="onButtonClick">
      Go Home
    </AppButton>
  </section>
</template>

<script>
import image from '@/assets/500.png'
import AppButton from '@/components/AppButton'

export default {
  name: 'Error500',
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
.error-500 {
  display: flex;
  flex-direction: column;
  align-items: center;

  &__image {
    max-width: 1000px;
    margin-bottom: 20px;
  }
}
</style>
