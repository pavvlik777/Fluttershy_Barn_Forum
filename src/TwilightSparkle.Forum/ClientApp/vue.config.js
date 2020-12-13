module.exports = {
  css: {
    loaderOptions: {
      sass: {
        additionalData: `@import "~@/styles";`
      }
    },
  },
  configureWebpack: {
    resolve: {
      mainFiles: ['Index.vue', 'index']
    }
  },
}