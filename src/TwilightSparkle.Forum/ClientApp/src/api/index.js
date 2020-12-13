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

// axios.defaults.baseURL = '/api'
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
      }
    }
  },
  heartbeat: {
    get: {
      date () {
        return axios.get('api/heartbeat/date')
      }
    }
  },
  users: {
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
