package com.newWebApp.service;
import org.springframework.stereotype.Service;

import com.newWebApp.pojo.User;



public interface IUserService {

    public User selectUser(int userId);

}