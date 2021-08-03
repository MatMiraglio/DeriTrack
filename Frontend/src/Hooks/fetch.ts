import useAxios from 'axios-hooks'
import { useEffect, useMemo } from 'react'

interface IState<T_DATA> {
    status: 'INVALID_URL' | 'FETCHING' | 'FETCHED' | 'ERROR'
    data?: T_DATA | undefined
    error?: any | undefined
}

interface IOutput<T_DATA> extends IState<T_DATA> {
    resend: () => any
}

export function useFetchGet<T_DATA>(url?: string, method: 'GET' | 'POST' = 'GET'): IOutput<T_DATA> {
    const [{ data, error, loading }, resend] = useAxios<T_DATA>({ url, method }, { manual: true })

    useEffect(() => {
        if(url) { // URL is a non-empty strings
            resend({ url })
        }
    }, [url, resend])

    return useMemo<IOutput<T_DATA>>(() => {
        if(url !== undefined) {
            if(loading === true) {
                return { status: 'FETCHING', resend }
            } else if(error) {
                return { status: 'ERROR', error, resend }
            } else {
                return { status: 'FETCHED', data, resend }
            }
        }
        return { status: 'INVALID_URL', resend }
    }, [url, data, error, loading, resend])
}
