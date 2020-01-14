import dateFormat from "dateformat";

const isMinDate = (date: Date|string) => {
    if(date instanceof Date) {
        return dateFormat(date, 'yyyy-mm-dd hh:MM:ss') === '1-01-01 12:00:00'
    } else {
        return date === '0001-01-01T00:00:00';
    }
}

export default isMinDate;