import Vue from 'vue'
import Vuex from 'vuex'
import api from '@/api'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
  },
  state: {
    userData: null,
    isLoading: false,
    isUserDataLoading: false,
    image: null
  },
  actions: {
    SET_USER_DATA: async function ({ commit }) {
      try {
        this.isUserDataLoading = true
        const response = await api.users.get.userData()
        const userData = response.data
        commit('SET_USER_DATA', { userData })
      } catch {
        commit('SET_USER_DATA', { userData: null })
      } finally {
        this.isUserDataLoading = false
      }
    }
  },
  mutations: {
    SET_USER_DATA: (state, { userData }) => {
      state.userData = userData
    },
    SET_LOADING: (state, isLoading) => {
      state.isLoading = isLoading
    },
    SET_MAIN_LOADING: (state, isLoading) => {
      state.isUserDataLoading = isLoading
    }
  },
  getters: {
    userData: state => state.userData,
    isLoading: state => state.isLoading,
    isUserDataLoading: state => state.isUserDataLoading,
    image: state => state.image
  }
})

export default store
