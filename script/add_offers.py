import json
import uuid
import random
import requests
from datetime import datetime, timedelta

SCHEDULE_MAP = {
    "mo": 0,
    "tu": 1,
    "we": 2,
    "th": 3,
    "fr": 4,
    "sa": 5,
    "su": 6
}

BASE_URL = "http://localhost:5000/api/offer"

def generate_dates_per_week(schedule):
    start_date = datetime(2025, 5, 8)
    end_date = start_date + timedelta(days=30)
    day_numbers = {SCHEDULE_MAP[d] for d in schedule}
    current = start_date
    dates_to_generate = []

    num_days = len(day_numbers)
    
    while current <= end_date:
        if current.weekday() in day_numbers:
            dates_to_generate.append(current)
        current += timedelta(days=1)

    if num_days == 1:
        return dates_to_generate[:1]
    elif num_days == 2 or num_days == 3:
        return dates_to_generate[:2]
    elif num_days >= 4:
        return dates_to_generate[:3]

    return dates_to_generate

def parse_duration(duration_str):
    hours = 0
    minutes = 0

    parts = duration_str.strip().split()

    for part in parts:
        if 'h' in part:
            hours = int(part.replace('h', '').strip())
        elif 'm' in part:
            minutes = int(part.replace('m', '').strip())

    return hours, minutes

def generate_price(hours, minutes, economy, first):
    base = (hours * 60 + minutes) * 1.5
    modifier = economy * 0.1 + first * 0.3
    return round(base - modifier, 2)

def generate_offers(data):
    offers = []

    for entry in data:
        if len(entry["airlines"]) == 0:
            airline = "Unknown Airline"
        else:
            airline = entry["airlines"][0]
        hours, minutes = parse_duration(entry["duration"])
        flight_duration = f"{hours}h {minutes}m"

        for date in generate_dates_per_week(entry["flight_schedule"]):
            economy = random.randint(80, 200)
            first = random.randint(5, min(30, economy // 2))
            departure_time = date.replace(hour=random.randint(0, 23), minute=random.choice([0, 15, 30, 45]))
            offer_id = str(uuid.uuid4())

            offer = {
                "Id": str(uuid.uuid4()),
                "Departure": f'{entry["origin"]} ({entry["origin_iata"].upper()})',
                "Arrival": f'{entry["destination"]} ({entry["destination_iata"].upper()})',
                "Price": generate_price(hours, minutes, economy, first),
                "Datetime": departure_time.strftime("%Y-%m-%dT%H:%M"),
                "Duration": flight_duration,
                "Airline": airline,
                "SeatsFirstClass": first,
                "SeatsEconomy": economy
            }

            offers.append((offer_id, offer))

    return offers

def send_offers(offers):
    total_offers = len(offers)
    for index, (offer_id, offer) in enumerate(offers, start=1): 
        url = f"{BASE_URL}/{offer_id}"
        response = requests.put(url, json=offer)
        
        if response.status_code in (200, 201):
            print(f"Wysłano {index}/{total_offers}")
        else:
            print(f"[ERR] {url} - {response.status_code}: {response.text}")


def main():
    with open("output.json", "r") as f:
        data = json.load(f)

    offers = generate_offers(data)
    send_offers(offers)

if __name__ == "__main__":
    main()
