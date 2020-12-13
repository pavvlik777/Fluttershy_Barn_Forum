import Vue from 'vue' 
import Router from 'vue-router'
import store from '@/store'

const MainLayout = () => import('@/layouts/MainLayout')
const Home = () => import('@/views/Home')
const Error404 = () => import('@/views/Errors/Error404')
const Error500 = () => import('@/views/Errors/Error500')
const Register = () => import('@/views/Register')
const Login = () => import('@/views/Login')
const ForumLayout = () => import('@/layouts/ForumLayout')

Vue.use(Router)

// const checkIfAuthorized = (to, from, next) => {
//   if (store.getters.userData) {
//     next()
//   } else {
//     next({ name: 'Error404'})
//   }
// }

// const checkIfAuthorizedAdmin = (to, from, next) => {
//   next()
// }

const routes = [
  {
    path: '/',
    beforeEnter: async function (to, from, next) {
      store.commit('SET_LOADING', true)
      await store.dispatch('SET_USER_DATA')
      store.commit('SET_LOADING', false)
      next()
    },
    component: MainLayout,
    children:  [
      {
        path: '',
        component: ForumLayout,
        name: 'ForumLayout',
        children: [
          {
            path: '',
            component: Home,
            name: 'Home'
          }
        ]
      },
      {
        path: '404',
        component: Error404,
        name: 'Error404'
      },
      {
        path: '500',
        component: Error500,
        name: 'Error500'
      },
      {
        path: 'register',
        component: Register,
        name: 'Register'
      },
      {
        path: 'login',
        component: Login,
        name: 'Login'
      }
    ]
  },
  {
    path: '*',
    redirect: { name: 'Error404' }
  }
]

const router = new Router({
  mode: 'history',
  fallback: false,
  routes
})

export default router
