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
  </div>
</template>

<script>
import userImage from '@/assets/user.png'

const formats = ['.jpg', '.png', '.jpeg']

export default {
  name: 'ForumLayoutUserCard',
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
}
</style>
