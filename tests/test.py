from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time

def login(driver, username, password):
    driver.find_element(By.ID, "username").send_keys(username)
    time.sleep(2)
    driver.find_element(By.ID, "password").send_keys(password)
    time.sleep(2)
    driver.find_element(By.XPATH, "//button[text()='Log in' and not(@disabled)]").click()
    time.sleep(2)
    try:
        driver.find_element(By.XPATH, "//h2[text()='Search for Flights']")
        print("Login successful")
    except:
        print("Login failed")


def select_flight(driver):
    driver.find_element(By.ID, "departure").send_keys("Paris")
    driver.find_element(By.ID, "arrival").send_keys("Milan")
    driver.find_element(By.ID, "from-date").send_keys("08-05-2025")
    driver.find_element(By.ID, "to-date").send_keys("15-05-2025")
    driver.execute_script("document.getElementById('price').value = 200;")
    driver.execute_script("document.getElementById('price').dispatchEvent(new Event('input'))")
    time.sleep(2)
    driver.find_element(By.XPATH, "//button[text()='Search']").click()
    time.sleep(2)
    driver.find_element(By.ID, "panel-0").click()
    time.sleep(2)
    driver.find_element(By.XPATH, "//button[text()='Check details']").click()
    time.sleep(2)

def select_details(driver):
    driver.find_element(By.ID, "first-class").clear()
    driver.find_element(By.ID, "first-class").send_keys("1")
    time.sleep(2)
    driver.find_element(By.ID, "economy-class").clear()
    driver.find_element(By.ID, "economy-class").send_keys("3")
    time.sleep(2)
    driver.find_element(By.ID, "carry-on-baggage").clear()
    driver.find_element(By.ID, "carry-on-baggage").send_keys("2")
    time.sleep(2)
    driver.find_element(By.ID, "checked-baggage").clear()
    driver.find_element(By.ID, "checked-baggage").send_keys("2")
    time.sleep(2)
    driver.find_element(By.ID, "priority-boarding").click()
    time.sleep(2)
    driver.find_element(By.XPATH, "//button[text()='Proceed']").click()
    time.sleep(2)
    driver.find_element(By.XPATH, "//button[normalize-space()='Confirm']").click()
    time.sleep(2)

def process_payment(driver):
    wait = WebDriverWait(driver, 10)

    while True:
        driver.find_element(By.XPATH, "//button[normalize-space()='Payment']").click()

        wait.until(EC.visibility_of_element_located((By.ID, "swal2-title")))
        message = driver.find_element(By.ID, "swal2-title").text.strip()

        if message == "Payment Accepted!":
            time.sleep(1)
            driver.find_element(By.XPATH, "//button[normalize-space()='Yes']").click()
            time.sleep(1)
            break

        elif message == "Payment Failed!":
            time.sleep(1)
            driver.find_element(By.XPATH, "//button[normalize-space()='OK']").click()
            time.sleep(2)
            
def main():
    options = webdriver.ChromeOptions()
    driver = webdriver.Chrome(options=options)
    driver.get("http://localhost:8080") 

    login(driver, "alice", "password")
    select_flight(driver)
    select_details(driver)
    process_payment(driver)



if __name__ == "__main__":
    main()
