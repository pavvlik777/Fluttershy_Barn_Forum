<template>
  <nav class="forum-layout-navigation">
    <h3> Sections </h3>
    <ul class="forum-layout-navigation__list">
      <li
        v-for="section in sections"
        :key="section.name"
        class="forum-layout-navigation__list-item"
      >
        <router-link
          :to="getSectionRoute(section.name)"
          class="forum-layout-navigation__link"
        >
          {{ section.name }}
        </router-link>
      </li>
    </ul>
  </nav>
</template>

<script>
export default {
  name: 'ForumLayoutNavigation',
  props: {
    sections: {
      type: Array,
      required: true
    }
  },
  methods: {
    getSectionRoute (name) {
      return {
        name: 'Section',
        params: { name }
      }
    }
  }
}
</script>

<style lang="scss">
.forum-layout-navigation {
  padding-left: 20px;

  &__list {
    margin: 0;
    padding: 5px 20px;
  }

  &__list-item {
    margin-bottom: 20px;
  }

  &__link {
    font-size: 14px;
    font-weight: 400;
    color: $black;
    padding: 5px;

    &.router-link-active {
      background-color: $fur-color-shadow;
      border: 3px solid $fur-border;
      border-radius: 6px;
    }
  }
}
</style>
