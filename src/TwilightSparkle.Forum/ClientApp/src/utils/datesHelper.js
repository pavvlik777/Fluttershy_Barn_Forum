const toUTCDateString = (dateString) => {
  const isUtcKind = dateString[dateString.length - 1] === 'Z' // check that date has ISO format like "2016-10-10T00:00:00.000Z"

  if (!isUtcKind) {
    dateString = dateString + 'Z'
  }

  return dateString
}

export default {
  getDateTimeFromDateString (dateString) {
    const date = new Date(toUTCDateString(dateString))

    return `${date.toLocaleDateString('en-GB')} ${date.toLocaleTimeString('it-IT', {
      hour: '2-digit',
      minute: '2-digit'
    })}`
  }
}
