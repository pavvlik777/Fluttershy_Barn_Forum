import Vue from 'vue'
import Vuex from 'vuex'
import api from '@/api'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
  },
  state: {
    userData: null,
    isUserDataLoading: false,
    roulette: {
      currentBet: 0,
      maxBetsCount: 0,
      isBetConfirmed: false,
      isGameFinished: false,
      bets: []
    }
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
    },
    SET_BET: (state, currentBet) => {
      state.roulette.isBetConfirmed = true
      state.roulette.currentBet = currentBet
      state.roulette.maxBetsCount = state.userData.moneyAmount / currentBet
    },
    RESET_BET: (state) => {
      state.roulette.isBetConfirmed = false
      state.roulette.currentBet = 0
      state.roulette.maxBetsCount = 0
      state.roulette.bets = []
      state.roulette.isGameFinished = false
    },
    SET_BETS: (state, bets)  => {
      state.roulette.bets = bets
    }
  },
  getters: {
    userData: state => state.userData,
    isUserDataLoading: state => state.isUserDataLoading,
    roulette: state => state.roulette
  }
})

export default store
