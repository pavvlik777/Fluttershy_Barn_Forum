import axios from 'axios'
import Vue from 'vue'
import qs from 'querystring'

axios.interceptors.request.use(function (config) {
  config.headers['Authorization'] = `Bearer ${Vue.$cookies.get('token')}`
  return config
}, function (error) {
  // Do something with request error
  return Promise.reject(error)
})

const clientData = {
  client_id : 'ro.client',
  client_secret: 'ro.client_secret_password_123'
}

axios.defaults.baseURL = '/api'
axios.defaults.headers.post['Content-Type'] = 'application/json'

const api = {
  authentication: {
    post: {
      signUp (model) {
        return axios.post('authentication/sign-up', model)
      },
      token (model) {
        const data = {
          ...clientData,
          ...model,
          grant_type: 'password'
        }

        return axios.post('connect/token', qs.stringify(data), {
          headers: {'Content-Type': 'application/x-www-form-urlencoded'}
        })
      },
      refreshToken () {
        const data = {
          ...clientData,
          grant_type: 'refresh_token',
          refresh_token: Vue.$cookies.get('refresh-token')
        }

        return axios.post('connect/token', qs.stringify(data), {
          headers: {'Content-Type': 'application/x-www-form-urlencoded'}
        })
      },
      confirmationMessage (model) {
        return axios.post('authentication/confirmation-message', model)
      },
      confirmEmail (model) {
        return axios.post('authentication/confirm-email', model)
      }
    }
  },
  heartbeat: {
    get: {
      date () {
        return axios.get('heartbeat/date')
      }
    }
  },
  users: {
    get: {
      userData () {
        return axios.get('users/data/current')
      },
      adminUserData (username) {
        return axios.get(`users/admin/data/${username}`)
      }
    },
    patch: {
      adminEmail (username, email) {
        return axios.patch(`users/admin/data/${username}/email`, { email })
      },
      passportImage (imageExternalId) {
        return axios.patch('users/data/current/passport-image', { imageExternalId })
      },
      adminPhoneNumber (username, phoneNumber) {
        return axios.patch(`users/admin/data/${username}/phone-number`, { phoneNumber })
      },
      phoneNumber (phoneNumber) {
        return axios.patch('users/data/current/phone-number', { phoneNumber })
      }
    },
    post: {
      block (username) {
        return axios.post(`users/admin/data/${username}/block`)
      },
      unblock (username) {
        return axios.post(`users/admin/data/${username}/unblock`)
      }
    },
    delete: {
      current () {
        return axios.delete('Users/data/current')
      }
    }
  },
  payments: {
    post: {
      deposit (model) {
        return axios.post('payments/deposit', model)
      },
      withdraw (model) {
        return axios.post('payments/withdraw', model)
      }
    }
  },
  blackjack: {
    post: {
      start (model) {
        return axios.post('blackjack-game/start', model)
      },
      hit (model) {
        return axios.post('blackjack-game/hit', model)
      },
      stand (model) {
        return axios.post('blackjack-game/stand', model)
      },
      processDealerBlackjack (model) {
        return axios.post('blackjack-game/process-dealer-blackjack', model)
      }
    }
  },
  balance: {
    get: {
      count (paymentMethod) {
        return axios.get('balance-stats/current/count', { params: { paymentMethod } })
      },
      info (startIndex, size, paymentMethod) {
        return axios.get('balance-stats/current/info', { params: {
          startIndex,
          size,
          paymentMethod
        }})
      }
    }
  },
  gameStats: {
    get: {
      count (title) {
        return axios.get('game-stats/current/count', { params: { title } })
      },
      info (startIndex, size, title) {
        return axios.get('game-stats/current/info', { params: {
          startIndex,
          size,
          title
        }})
      }
    }
  },
  roulette: {
    post: {
      play (model) {
        return axios.post('roulette-game/play', model)
      }
    }
  }
}

axios.interceptors.response.use(function (response) {
  return response
}, async function (error) {
  if (error.response.status === 401) {
    const response = await api.authentication.post.refreshToken()
    // Vue.$store.commit('SET_USER_DATA', { userData: null })
    
    const { access_token, refresh_token, expires_in } = response.data
    Vue.$cookies.set('token', access_token, expires_in)
    Vue.$cookies.remove('refresh-token')
    Vue.$cookies.set('refresh-token', refresh_token, 60 * 60 * 24 * 30)
    // await Vue.$store.dispatch('SET_USER_DATA')

    return axios(error.config)
  }

  return Promise.reject(error)
})

export default api
