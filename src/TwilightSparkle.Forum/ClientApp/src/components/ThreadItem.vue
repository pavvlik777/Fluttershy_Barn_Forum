<template>
  <div
    class="section-thread"
    @click="$emit('click', id)"
  >
    <div class="section-thread__column">
      <h4 class="section-thread__title">
        {{ title }}
      </h4>
      <span
        v-if="likesDislikes !== null"
        :class="likesClass"
        class="section-thread__likes"
      >
        {{ likesDislikes }}
      </span>
    </div>
    <div class="section-thread__column">
      <p> 
        {{ dateTime }}
      </p>
      <span v-if="authorUsername"> By: {{ authorUsername }} </span>
    </div>
  </div>
</template>

<script>
import { datesHelper } from '@/utils'

export default {
  name: 'SectionThread',
  props: {
    id: {
      type: Number,
      reuqired: true
    },
    title: {
      type: String,
      required: true
    },
    authorUsername: {
      type: String,
      default: ''
    },
    creationDateTimeUtc: {
      type: String,
      required: true
    },
    likesDislikes: {
      type: Number,
      default: null
    }
  },
  computed: {
    dateTime () {
      return datesHelper.getDateTimeFromDateString(this.creationDateTimeUtc)
    },
    likesClass () {
      if (this.likesDislikes > 0) {
        return 'section-thread__likes--positive'
      }

      if (this.likesDislikes < 0) {
        return 'section-thread__likes--negative'
      }

      return ''
    }
  }
}
</script>

<style lang="scss">
.section-thread {
  display: flex;
  justify-content: space-between;
  border: 1px solid $mane-border;
  background-color: $mane-color;
  padding: 5px 10px;
  border-radius: 6px;
  cursor: pointer;

  h4 {
    margin: 10px 0;
  }

  &__column {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }

  &__likes {
    font-weight: 700;
    font-size: 20px;

    &--positive {
      color: green;
    }

    &--negative {
      color: red;
    }
  }
}
</style>
