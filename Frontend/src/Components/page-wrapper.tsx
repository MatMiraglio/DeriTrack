import React, { useMemo } from 'react'
import { useTheme, useMediaQuery } from '@material-ui/core'
import { Breakpoint } from '@material-ui/core/styles/createBreakpoints'

interface Props {
    maxWidth: Breakpoint
    children: any
}


export default function PageWrapper({ children, maxWidth }: Props) {
    const theme = useTheme()

    const margin = theme.spacing(1.5)
    const maxWidthValue = theme.breakpoints.values[maxWidth]

    const matches = useMediaQuery(`(min-width:${maxWidthValue + (2 * margin)}px)`)

    const style = useMemo(() => ({
        margin: matches ? `${margin * 2}px auto` : `${margin * 2}px ${margin}px`,
        maxWidth: maxWidthValue,
    }), [matches, maxWidthValue, margin])

    return <div
        style={style}
        children={children}
    />
}