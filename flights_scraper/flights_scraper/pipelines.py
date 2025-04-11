# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://docs.scrapy.org/en/latest/topics/item-pipeline.html


# useful for handling different item types with a single interface
from itemadapter import ItemAdapter
import re


class FlightsScraperPipeline:
    def process_item(self, item, spider):
        adapter = ItemAdapter(item)

        if adapter.get("duration"):
            duration = re.sub(r'(\d+)\s*hours?', r'\1h', adapter["duration"])
            duration = re.sub(r'(\d+)\s*minutes?', r'\1m', duration)
            duration = duration.replace(' and ', ' ')
            item['duration'] = duration.strip()

        return item
