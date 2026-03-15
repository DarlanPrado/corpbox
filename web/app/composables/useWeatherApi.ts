import { z } from 'zod'
import { schemas } from '~/generated/openapi.zod'
import type { components } from '~/generated/openapi.types'

type WeatherForecast = components['schemas']['WeatherForecast']
const weatherForecastListSchema = z.array(schemas.WeatherForecast)

export function useWeatherApi() {
  const runtimeConfig = useRuntimeConfig()

  async function getForecast(): Promise<WeatherForecast[]> {
    const payload = await $fetch<unknown>('/WeatherForecast', {
      method: 'GET',
      baseURL: runtimeConfig.public.apiBaseUrl,
      credentials: 'include'
    })

    return weatherForecastListSchema.parse(payload)
  }

  return {
    getForecast
  }
}
