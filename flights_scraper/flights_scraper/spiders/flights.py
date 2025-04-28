import scrapy

from flights_scraper.items import Flight


class FlightsSpider(scrapy.Spider):
    name = "flights"
    start_urls = ["https://www.flightconnections.com"]

    def start_requests(self):
        base_url = "https://www.flightconnections.com"

        iata_codes = [
            "atl", "pek", "lhr", "hnd", "dfw", "cdg", "den", "fra", "hkg", "sin",
            "ams", "ist", "cvg", "jfk", "lax", "ord", "mco", "mia", "muc", "sfo",
            "clt", "las", "phx", "sea", "iad", "bkk", "doh", "gru", "mad", "syd",
            "yyz", "bom", "del", "jnb", "lga", "fco", "zrh", "vko", "svo", "arn",
            "osl", "cph", "hel", "waw", "prg", "bud", "vie", "bru", "dxb", "kix",
            "pvg", "sha", "icn", "nrt", "gmp", "bne", "mel", "per", "akl", "gva",
            "mxp", "lhr", "lgw", "man", "bhx", "edi", "bfs", "dub", "rdu", "aus",
            "msy", "tpa", "fll", "pbi", "sju", "nas", "mbj", "canc", "scl",
            "eze", "lim", "bog", "mde", "ctg", "pty", "mga", "sjo", "sal", "gua",
            "sap", "tgu", "bze", "cuz", "lpb", "uio", "gye", "gig", "for", "rec"
        ]

        for code1 in iata_codes:
            for code2 in iata_codes:
                if code1 != code2:
                    url = f"{base_url}/flights-from-{code1}-to-{code2}"
                    yield scrapy.Request(url, self.parse, meta={"code1": code1, "code2": code2})      

    def parse(self, response):
        flight = Flight()

        flight["duration"] = response.css("div.route-page-info-row:nth-child(3) > div:nth-child(2) > div:nth-child(2) > span:nth-child(1)::text").get()

        if flight["duration"] is None:
            pass
        else:
            flight["origin_iata"] = response.meta.get("code1")
            flight["origin"] = response.css("div.route-page-dep  h3::text").get()
            flight["destination_iata"] = response.meta.get("code2")
            flight["destination"] = response.css("div.route-page-des h3::text").get()
            flight["airlines"] = response.css("ul.route-page-info-text.airlines li::text").getall()
            flight["flight_schedule"] = response.css("div.schedule-day::text").getall()
            flight["aircrafts"] = response.css("ul.route-page-info-text.aircrafts li::text").getall()
            yield flight
