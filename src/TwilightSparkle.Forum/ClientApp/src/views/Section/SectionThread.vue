<template>
  <div
    class="section-thread"
    @click="$emit('click', id)"
  >
    <h4 class="section-thread__title">
      {{ title }}
    </h4>
    <div class="section-thread__column">
      <p> {{ dateTime }} </p>
      <span> By: {{ authorUsername }} </span>
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
      required: true
    },
    creationDateTimeUtc: {
      type: String,
      required: true
    }
  },
  computed: {
    dateTime () {
      return datesHelper.getDateTimeFromDateString(this.creationDateTimeUtc)
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

  &__column {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
}
</style>
