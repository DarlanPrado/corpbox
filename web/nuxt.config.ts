// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: ['@nuxtjs/i18n'],
  runtimeConfig: {
    public: {
      apiBaseUrl: 'http://localhost:5003'
    }
  },
  i18n: {
    strategy: 'prefix_and_default',
    defaultLocale: 'pt',
    detectBrowserLanguage: false,
    bundle: {
      optimizeTranslationDirective: false
    },
    locales: [
      {
        code: 'pt',
        language: 'pt-BR',
        name: 'Portugues'
      },
      {
        code: 'en',
        language: 'en-US',
        name: 'English'
      }
    ]
  }
})
