import Vue from 'vue'
import Vuex from 'vuex'
import api from '@/api'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
  },
  state: {
    userData: null,
    isUserDataLoading: false
  },
  actions: {
    SET_USER_DATA: async function ({ commit }) {
      try {
        const response = await api.users.get.userData()
        const userData = response.data
        commit('SET_USER_DATA', { userData })
      } catch {
        commit('SET_USER_DATA', { userData: null })
      }
    }
  },
  mutations: {
    SET_USER_DATA: (state, { userData }) => {
      state.userData = userData
    },
    SET_LOADING: (state, isLoading) => {
      state.isUserDataLoading = isLoading
    }
  },
  getters: {
    userData: state => state.userData,
    isUserDataLoading: state => state.isUserDataLoading
  }
})

export default store
