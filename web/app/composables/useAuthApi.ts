import { z } from 'zod'
import { schemas } from '~/generated/openapi.zod'
import type { components } from '~/generated/openapi.types'

type LoginRequest = components['schemas']['LoginRequest']
type RegisterRequest = components['schemas']['RegisterRequest']

function getErrorMessage(error: unknown): string {
  if (error instanceof z.ZodError) {
    return error.issues[0]?.message ?? 'Invalid request payload.'
  }

  if (error && typeof error === 'object' && 'data' in error) {
    const errorData = (error as { data?: { message?: string } }).data
    if (errorData?.message) {
      return errorData.message
    }
  }

  if (error instanceof Error) {
    return error.message
  }

  return 'Unexpected API error.'
}

export function useAuthApi() {
  const runtimeConfig = useRuntimeConfig()

  async function login(payload: LoginRequest) {
    const body = schemas.LoginRequest.parse(payload)

    try {
      return await $fetch('/api/Auth/login', {
        method: 'POST',
        baseURL: runtimeConfig.public.apiBaseUrl,
        body,
        credentials: 'include'
      })
    } catch (error) {
      throw new Error(getErrorMessage(error))
    }
  }

  async function register(payload: RegisterRequest) {
    const body = schemas.RegisterRequest.parse(payload)

    try {
      return await $fetch('/api/Auth/register', {
        method: 'POST',
        baseURL: runtimeConfig.public.apiBaseUrl,
        body,
        credentials: 'include'
      })
    } catch (error) {
      throw new Error(getErrorMessage(error))
    }
  }

  return {
    login,
    register
  }
}
