<template>
  <article class="create-thread">
    <div class="create-thread__title">
      <label> Title: </label>
      <AppInput v-model="title" />
    </div>
    <label> Content: </label>
    <textarea
      v-model="content"
      class="create-thread__textarea"
    />
    <AppButton @click="onCreate">
      Create
    </AppButton>
  </article>
</template>

<script>
import AppInput from '@/components/AppInput'
import AppButton from '@/components/AppButton'
import api from '@/api'

export default {
  components: {
    AppInput,
    AppButton
  },
  name: 'CreateThread',
  props: {
    sectionName: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      title: '',
      content: ''
    }
  },
  methods: {
    async onCreate () {
      this.$store.commit('SET_LOADING', true)
      try {
        const response = await api.threads.post.create({
          title: this.title,
          content: this.content,
          sectionName: this.sectionName
        })
        this.$router.push({
          name: 'Thread',
          params: {
            sectionName: this.sectionName,
            id: response.data.id
          }
        })
      } catch {
        this.$route.replace({ name: 'Error500' })
      } finally {
        this.$store.commit('SET_LOADING', false)
      }
    }
  }
}
</script>

<style lang="scss">
.create-thread {
  display: flex;
  flex-direction: column;
  padding: 20px 30px;

  & > * + * {
    margin-top: 15px;
  }

  &__title {
    display: flex;
    align-items: center;
    .app-input {
      margin-left: 15px;
    }
  }

  &__textarea {
    resize: vertical;
    min-height: 300px;
    padding: 10px;
    border-radius: 4px;
  }
}
</style>
