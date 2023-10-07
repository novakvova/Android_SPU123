package com.example.mystore;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.ImageView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;
import com.example.mystore.application.HomeApplication;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ImageView ivLogo = findViewById(R.id.ivLogo);
//        String url = "https://img.freepik.com/free-photo/happiness-wellbeing-and-confidence-concept-cheerful-attractive-african-american-woman-curly-haircut-cross-arms-chest-in-self-assured-powerful-pose-smiling-determined-wear-yellow-sweater_176420-35063.jpg?w=360";
//        String url = "http://10.0.2.2:5190/images/1.jpg";
        String url = "https://spu123.itstep.click/images/1.jpg";
        Glide
                .with(HomeApplication.getAppContext())
                .load(url)
                .apply(new RequestOptions().override(600))
                .into(ivLogo);


    }
}