<template>
  <div class="forum-layout-user-card">
    <aside class="forum-layout-user-card__card">
      <div class="forum-layout-user-card__image-container">
        <input
          v-if="userData"
          :accept="formats.join(',')"
          class="forum-layout-user-card__input"
          ref="fileInput"
          type="file"
          @change="fileChanged"
        >
        <img
          :src="imageOrDefault"
          class="forum-layout-user-card__image"
        >
      </div>
      <span class="forum-layout-user-card__text">
        {{ usernameOrDefault }}
      </span>
      <span
        v-if="userData"
        class="forum-layout-user-card__text"
      >
        Email: {{ userData.email }}
      </span>
    </aside>
    <ul class="forum-layout-user-card__list">
      <li
        v-for="thread in threads"
        :key="thread.id"
        class="forum-layout-user-card__list-item"
      >
        <SectionThread
          v-bind="thread"
          @click="() => onOpenThread(thread)"
        />
      </li>
      <li
        v-if="isAdditionalLoadRequired"
        class="forum-layout-user-card__item-loading loading"
      />
    </ul>
  </div>
</template>

<script>
import SectionThread from '@/components/ThreadItem'
import userImage from '@/assets/user.png'

const formats = ['.jpg', '.png', '.jpeg']

export default {
  name: 'ForumLayoutUserCard',
  components: {
    SectionThread
  },
  props: {
    threads: {
      type: Array,
      default: () => []
    },
    isAdditionalLoadRequired: {
      type: Boolean,
      deafult: false
    }
  },
  data () {
    return {
      formats
    }
  },
  computed: {
    userData () {
      return this.$store.getters.userData
    },
    usernameOrDefault () {
      return this.userData
        ? `Name: ${this.userData.username}`
        : 'Guest'
    },
    imageOrDefault () {
      return this.userData && this.userData.profileImageExternalId
        ? `/api/images/${this.userData.profileImageExternalId}`
        : userImage
    }
  },
  methods: {
    onFileLoad () {
      this.$emit('input', this.$refs.fileInput.files[0])
    },
    fileChanged (e) {
      if (!e.target.files.length) {
        return
      }

      const formData = e.target.files[0]
      this.uploadFileName = formData.name
      const reader = new FileReader()
      reader.onload = this.onFileLoad
      reader.readAsDataURL(formData)
    },
    onOpenThread (thread) {
      this.$router.push({
        name: 'Thread',
        params: {
          sectionName: thread.sectionName,
          id: thread.id
        }
      })
    }
  }
}
</script>

<style lang="scss">
.forum-layout-user-card {
  padding: 30px 20px;

  &__card {
    border: 3px dotted $fur-border;
    display: flex;
    flex-direction: column;
    padding: 20px 10px;
  }

  &__image {
    width: 100px;
    border-radius: 100px;
  }

  &__text {
    margin: 10px 0;
    font-size: 14px;
    font-weight: 500;
  }

  &__input {
    opacity: 0;
    position: absolute;
    left: 0;
    width: 100%;
    height: 100%;
    cursor: pointer;
  }

  &__image-container {
    position: relative;
    align-self: center;
  }

  &__item-loading.loading {
    display: block;
    width: 300px;
    height: 335px;
    position: relative;

    &::before {
      transform: scale(0.5);
    }
  }

  &__list {
    list-style-type: none;
    padding: 0;
  }

  &__list-item {
    margin: 5px 0;
  }
}
</style>
