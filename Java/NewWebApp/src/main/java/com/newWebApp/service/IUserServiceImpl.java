package com.newWebApp.service;

import com.newWebApp.dao.IUserDao;
import com.newWebApp.mapper.UserMapper;
import com.newWebApp.pojo.User;
import org.springframework.stereotype.Service;

import org.springframework.beans.factory.annotation.Autowired;

@Service
public class IUserServiceImpl implements IUserService {

    @Autowired
    private UserMapper userMapper;

    public User selectUser(int userId){
        return this.userMapper.selectByPrimaryKey(userId);
    }

}