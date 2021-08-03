import { useTheme } from '@material-ui/core'
import React from 'react'

interface IProps {
    /**
     * The value can be number or String:
     *  - Number: This will be equal to theme.spacing(value)
     *  - String: This will be mapped to style attribute. example: 3rem
     *
     * @default 3
     */
    amount?: string | number
}

/**
 * Render a vertical spacer.
 */
export default function Spacer({ amount = 3 }: IProps) {
    const theme = useTheme()

    const height = typeof(amount) === 'string' ? amount : theme.spacing(amount)

    return <div style={{ height, width: '100%' }}/>
}
