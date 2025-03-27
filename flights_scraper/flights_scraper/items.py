# Define here the models for your scraped items
#
# See documentation in:
# https://docs.scrapy.org/en/latest/topics/items.html

import scrapy


class Flight(scrapy.Item):
    origin = scrapy.Field()
    destination = scrapy.Field()
    duration = scrapy.Field()
    airlines = scrapy.Field()
    aircrafts = scrapy.Field()
    flight_schedule = scrapy.Field()

