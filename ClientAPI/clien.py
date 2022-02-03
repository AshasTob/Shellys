import requests as req
import json


class BarApiClient:
    def __init__(self, link):
        self.link = link

    def check_code(self, code):
        return 200 <= code < 300

    def GetMenu(self):
        apiLink = self.link + "/api/menu"

        resp = req.get(apiLink)

        if self.check_code(resp.status_code):
            deser = json.loads(resp.text)
            return deser
        else:
            return None

    def GetDrink(self, drinkName):
        apiLink = self.link + "/api/menu/" + drinkName

        resp = req.get(apiLink)
        if self.check_code(resp.status_code):
            return json.load(resp.text)
        else:
            return None

    def Add(self, name, price):
        apiLink = self.link + "/api/menu"

        pload = json.dumps({'name': name, 'price': price})
        headers = {"accept": "*/*", "Content-Type": "application/json"}

        resp = req.post(apiLink, headers=headers, data=pload)
        if self.check_code(resp.status_code):
            print(f"Added {name}")
        else:
            print(resp)

    def ChangePrice(self, name, newPrice):
        apiLink = self.link + "/api/menu/" + name

        pload = json.dumps({'price': newPrice})
        headers = {"accept": "*/*", "Content-Type": "application/json"}

        resp = req.post(apiLink, headers=headers, data=pload)
        if self.check_code(resp.status_code):
            print(f"Changed price of {name} to {newPrice}")
        else:
            print(resp)

    def DeleteDrink(self, name):
        apiLink = self.link + "/api/menu/" + name

        resp = req.delete(apiLink)
        if self.check_code(resp.status_code):
            print(f"Deleted {name}")
        else:
            print(resp)
