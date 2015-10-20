#!/usr/bin/env python
import MySQLdb
from urlparse import urlparse

#display all orderIDs in table
def display():
    try:
        cursor.execute("SELECT * FROM whiteListed_order;")
        order_ids = cursor.fetchall()
        print "White Listed OrderIds: "
        for order_id in order_ids:
            print("{}".format (order_id[0]))
    except MySQLdb.Error as e:
        print "Unable to fetch data"        

#insert multiple orderIDs
def insert():
    input = raw_input("Enter multiple orderIds you want to insert, separated by commas: ")
    orderIDs_list = [order_id.strip(' ') for order_id in input.split(',')]
    integerCheck = all([order_id.replace(' ','').isdigit() for order_id in orderIDs_list])
    if(integerCheck):
        try:
            insert = "INSERT INTO whiteListed_order" "(order_id)" "VALUES (%(order_id)s)"     
            cursor.executemany(insert, [{'order_id': order_id} for order_id in orderIDs_list])
            db.commit()
            print "OrderId: {} has been inserted into database successfully".format(orderIDs_list) 
        except MySQLdb.Error as e:
            db.rollback()
            print (e)
    else:
        print "Your entry contains invalid OrderId"

#delete multiple orderIDs
def delete():
    input = raw_input("Enter multiple orderIds you want to delete, separated by commas: ")
    orderIDs_list = [order_id.strip(' ') for order_id in input.split(',')]
    integerCheck = all([order_id.replace(' ','').isdigit() for order_id in orderIDs_list])
    if(integerCheck):
        try:
            delete = "DELETE FROM whiteListed_order WHERE order_id in (%(order_id)s)"
            orderId = {'order_id':order_id}
            cursor.executemany(delete, [{'order_id': order_id} for order_id in orderIDs_list])
            db.commit()
            print "OrderId: {} has been removed into database successfully".format(orderIDs_list) 
        except MySQLdb.Error as e:
            db.rollback()
            print(e)
    else:    
        print "Your entry contains invalid OrderId"

#delete all orderIDs 
def delete_all():
    input = raw_input("Are you sure you want to delete all OrderIDs? [Y/N] ")
    if input == 'Y' or input == 'y':
        try:
            delete = "TRUNCATE TABLE whiteListed_order "    
            cursor.execute(delete)
            db.commit()
            print "All OrderIDs have been removed from database."
        except MySQLdb.Error as e:
            db.rollback()
            print "Error: unable to delete all records" 

def notAfun():
    print "Error: not a valid function name, please enter again"    


#also check if the url is changing when we deploy it    
def main():
    try:
        global db 
        global cursor
       #following fields will be replaced in production
       #db = MySQLdb.connect(host, user, passwd, db)        
        db = MySQLdb.connect("localhost", "root", "password", "dmfulfillment")
        cursor = db.cursor()
    except MySQLdb.Error as e:
        print(e)    

    while True:         
        choice = raw_input('choose from: display, insert, delete, delete_all or quit: ')
        if choice == "quit":
            break   
        {
        'display': display,
        'insert': insert,
        'delete': delete,
        'delete_all': delete_all
        }.get(choice, notAfun)()

    cursor.close()
    db.close()


if __name__ == '__main__':
    main()
