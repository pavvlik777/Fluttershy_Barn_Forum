import Vue from 'vue'

export default {
  async sendData (file, callback) {
    const formData = new FormData();

    formData.append("image", file)

    const request = new XMLHttpRequest()

    try {
      request.open('POST', `/api/images/upload`)
      request.setRequestHeader('Authorization', `Bearer ${Vue.$cookies.get('token')}`)
      request.send(formData)
    } catch (e) {
      console.log(e)
      window.location.reload()
    }

    request.onreadystatechange = function() {
      if (request.readyState === 4) {
        callback(this.response)
      }
    }
  }
}