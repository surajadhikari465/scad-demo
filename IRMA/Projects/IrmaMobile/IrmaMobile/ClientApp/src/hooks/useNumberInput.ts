import { useState, useEffect } from 'react'

const allNumbersRegex = /^(-\d+|\d+|-\d+\.\d|\d+\.\d|-\d+\.\d\d|\d+\.\d\d|-\.\d{1,2}|\.\d{1,2}){1}$/;
const positiveWithDecimalNumberRegex = /^(\d+|\d+\.\d|\d+\.\d\d|\.\d{1,2}){1}$/;
const negativeWithNoDecimalNumberRegex = /^-{0,1}\d+$/;
const positiveWithNoDecimalNumberRegex = /^\d+$/

const useNumberInput = (
    initialValue: number,
    initialValueInput: string,
    initialAllowDecimals: boolean,
    initialAllowNegativeNumbers: boolean,
    initialErrorMessage: string) => {

    const [value, setValue] = useState<number>(initialValue);
    const [valueInput, setValueInput] = useState<string>(initialValueInput);
    const [allowDecimals, setAllowDecimals] = useState<boolean>(initialAllowDecimals);
    const [allowNegativeNumbers, setAllowNegativeNumbers] = useState<boolean>(initialAllowNegativeNumbers);
    const [errorMessage, setErrorMessage] = useState<string>(initialErrorMessage);

    useEffect(() => {
        if (valueInput === '') {
            setValue(0);
            setErrorMessage('');
            return;
        }
        if (allowDecimals) {
            if (allowNegativeNumbers) {
                if (allNumbersRegex.test(valueInput)) {
                    setErrorMessage('');
                    setValue(Number(valueInput));
                } else {
                    setValue(0);
                    setErrorMessage('Please enter a number with up to two decimal places.');
                }
            } else {
                if (positiveWithDecimalNumberRegex.test(valueInput)) {
                    setErrorMessage('');
                    setValue(Number(valueInput));
                } else if (valueInput.includes('-')) {
                    setValue(0);
                    setErrorMessage('Please enter a non-negative number with up to two decimal places.');
                }  else {
                    setValue(0);
                    setErrorMessage('Please enter a non-negative number with up to two decimal places.');
                }
            }
        } else if (allowNegativeNumbers) {
            if (negativeWithNoDecimalNumberRegex.test(valueInput)) {
                setErrorMessage('');
                setValue(Number(valueInput));
            } else {
                setValue(0);
                setErrorMessage('Please enter a number with no decimals.');
            }
        } else {
            if (positiveWithNoDecimalNumberRegex.test(valueInput)) {
                setErrorMessage('');
                setValue(Number(valueInput));
            } else if (valueInput.includes('-')) {
                setValue(0);
                setErrorMessage('Please enter a non-negative number with no decimals.');
            } else {
                setValue(0);
                setErrorMessage('Please enter a number with no decimals.');
            }
        }
    }, [valueInput, allowDecimals, allowNegativeNumbers])

    return { value, setValue, valueInput, setValueInput, allowDecimals, setAllowDecimals, allowNegativeNumbers, setAllowNegativeNumbers, errorMessage, setErrorMessage };
}

export default useNumberInput;